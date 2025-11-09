/**
 * Video Upload Component
 * @module components/Upload
 */

'use client';

import { memo, useCallback, useState } from 'react';
import { useRouter } from 'next/navigation';
import { uploadVideo, validateVideoFile } from '@/lib/api/upload';
import { useUploadStore } from '@/store/upload';
import { useAuthStore } from '@/store/auth';
import { cn } from '@/lib/utils';

/**
 * Video Upload Component
 */
const VideoUpload = memo(() => {
  const router = useRouter();
  const { isAuthenticated } = useAuthStore();
  const { progress, isUploading, setProgress, setUploading, setError, reset } = useUploadStore();
  
  const [dragActive, setDragActive] = useState(false);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [caption, setCaption] = useState('');
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  const handleFileSelect = useCallback((file: File) => {
    const validation = validateVideoFile(file);
    
    if (!validation.valid) {
      setError(validation.error || 'Invalid file');
      return;
    }

    setSelectedFile(file);
    setError(null);

    // Create preview URL
    const url = URL.createObjectURL(file);
    setPreviewUrl(url);
  }, [setError]);

  const handleDrag = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    
    if (e.type === 'dragenter' || e.type === 'dragover') {
      setDragActive(true);
    } else if (e.type === 'dragleave') {
      setDragActive(false);
    }
  }, []);

  const handleDrop = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);

    const files = e.dataTransfer.files;
    if (files && files[0]) {
      handleFileSelect(files[0]);
    }
  }, [handleFileSelect]);

  // Redirect if not authenticated
  if (!isAuthenticated) {
    router.push('/login');
    return null;
  }

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files && files[0]) {
      handleFileSelect(files[0]);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!selectedFile) {
      setError('Please select a video file');
      return;
    }

    setUploading(true);
    setError(null);

    try {
      await uploadVideo(
        {
          file: selectedFile,
          caption,
        },
        (progress) => {
          setProgress(progress);
        }
      );

      // Success - redirect to home
      reset();
      setSelectedFile(null);
      setCaption('');
      if (previewUrl) {
        URL.revokeObjectURL(previewUrl);
        setPreviewUrl(null);
      }
      router.push('/');
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Upload failed';
      setError(message);
    }
  };

  const handleCancel = () => {
    if (previewUrl) {
      URL.revokeObjectURL(previewUrl);
    }
    setSelectedFile(null);
    setPreviewUrl(null);
    setCaption('');
    reset();
  };

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <h1 className="text-3xl font-bold mb-8">Upload Video</h1>

        {!selectedFile ? (
          <div
            onDragEnter={handleDrag}
            onDragLeave={handleDrag}
            onDragOver={handleDrag}
            onDrop={handleDrop}
            className={cn(
              'border-2 border-dashed rounded-lg p-12 text-center transition-colors',
              dragActive ? 'border-blue-500 bg-blue-50' : 'border-gray-300 bg-white'
            )}
          >
            <svg
              className="w-16 h-16 mx-auto mb-4 text-gray-400"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"
              />
            </svg>

            <h3 className="text-xl font-semibold mb-2">
              {dragActive ? 'Drop video here' : 'Select video to upload'}
            </h3>
            <p className="text-gray-600 mb-6">Or drag and drop a file</p>

            <label className="inline-block px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold cursor-pointer hover:bg-blue-700 transition-colors">
              Select File
              <input
                type="file"
                accept="video/mp4,video/webm,video/quicktime"
                onChange={handleFileChange}
                className="hidden"
              />
            </label>

            <p className="text-sm text-gray-500 mt-4">
              MP4, WebM, or MOV • Max 50MB • Up to 3 minutes
            </p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="bg-white rounded-lg shadow-lg p-6">
            <div className="grid md:grid-cols-2 gap-6">
              {/* Video Preview */}
              <div>
                <h3 className="font-semibold mb-3">Preview</h3>
                {previewUrl && (
                  <video
                    src={previewUrl}
                    controls
                    className="w-full rounded-lg bg-black"
                  />
                )}
                <p className="text-sm text-gray-600 mt-2">{selectedFile.name}</p>
              </div>

              {/* Upload Form */}
              <div>
                <h3 className="font-semibold mb-3">Details</h3>
                
                <div className="mb-4">
                  <label htmlFor="caption" className="block text-sm font-medium mb-2">
                    Caption
                  </label>
                  <textarea
                    id="caption"
                    value={caption}
                    onChange={(e) => setCaption(e.target.value)}
                    placeholder="Describe your video..."
                    rows={4}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                    maxLength={150}
                  />
                  <p className="text-xs text-gray-500 mt-1">{caption.length}/150</p>
                </div>

                {isUploading && (
                  <div className="mb-4">
                    <div className="flex justify-between text-sm mb-1">
                      <span>Uploading...</span>
                      <span>{progress}%</span>
                    </div>
                    <div className="w-full bg-gray-200 rounded-full h-2">
                      <div
                        className="bg-blue-600 h-2 rounded-full transition-all"
                        style={{ width: `${progress}%` }}
                      />
                    </div>
                  </div>
                )}

                <div className="flex space-x-3">
                  <button
                    type="submit"
                    disabled={isUploading}
                    className="flex-1 px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
                  >
                    {isUploading ? 'Uploading...' : 'Upload'}
                  </button>
                  <button
                    type="button"
                    onClick={handleCancel}
                    disabled={isUploading}
                    className="px-6 py-3 bg-gray-200 text-gray-800 rounded-lg font-semibold hover:bg-gray-300 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </div>
          </form>
        )}
      </div>
    </div>
  );
});

VideoUpload.displayName = 'VideoUpload';

export default VideoUpload;
