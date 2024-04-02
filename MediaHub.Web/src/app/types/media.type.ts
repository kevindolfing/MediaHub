export type Media = {
  name: string;
  path: string;
  thumbnailUrl?: string;
  type: 0 | 1; // 0 = folder, 1 = file
  children: Media[];
};
