// src/app/api/upload/route.ts
import { NextRequest, NextResponse } from 'next/server';
import { promises as fs } from 'fs';
import path from 'path';
import formidable from 'formidable';

export const config = {
  api: {
    bodyParser: false,
  },
};

export async function POST(request: NextRequest) {
  const form = formidable({});

  try {
    const [_fields, files] = await form.parse(request as any);
    const file = files.file?.[0];

    if (!file) {
      return NextResponse.json({ message: 'No file uploaded' }, { status: 400 });
    }

    const uploadDir = path.join(process.cwd(), 'public', 'uploads');
    await fs.mkdir(uploadDir, { recursive: true });

    const newPath = path.join(uploadDir, file.originalFilename || file.newFilename);
    await fs.rename(file.filepath, newPath);

    const videoUrl = `/uploads/${file.originalFilename || file.newFilename}`;

    // Here you would save the videoUrl to your database
    console.log('File uploaded to:', videoUrl);

    return NextResponse.json({ message: 'File uploaded successfully', url: videoUrl });
  } catch (error) {
    console.error('Upload error:', error);
    return NextResponse.json({ message: 'An error occurred during upload' }, { status: 500 });
  }
}
