// src/components/Feed.tsx
import VideoPlayer from './Video';

const videos = [
  {
    id: 1,
    src: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4',
    poster: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/BigBuckBunny.jpg',
  },
  {
    id: 2,
    src: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4',
    poster: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ElephantsDream.jpg',
  },
  {
    id: 3,
    src: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4',
    poster: 'http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ForBiggerBlazes.jpg',
  },
];

const Feed = () => {
  return (
    <div className="h-screen w-full snap-y snap-mandatory overflow-y-scroll">
      {videos.map((video) => (
        <VideoPlayer key={video.id} id={video.id.toString()} src={video.src} poster={video.poster} />
      ))}
    </div>
  );
};

export default Feed;
