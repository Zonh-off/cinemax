export type Movie = {
  id: number;
  title: string;
  posterPath: string;
}

export type Pagination<T> = {
  pageNumber: number,
  pageSize: number,
  totalItems: number
  data: T[]
}

export type MovieDetails = {
  id: number;
  title: string;
  status: string;
  overview: string;
  posterPath: string;
  voteAverage: number;
  voteCount: number;
  runtime: number;
  genres: string[];
  director: string;
  actors: Cast[];
  trailerUrl: string;
  images: string[];
  videos: Video[];
  ratingDistribution: { [key: number]: number };
}

export type Cast = {
  name: string;
  character: string;
  profilePath: string;
}

export type Video = {
  name: string; key:
  string; site: string;
  type: string;
}

export class ShowtimesParams {
  pageNumber: number = 1;
  pageSize: number = 6;
  cityId: number | undefined;
  cinemaId: number | undefined;
  from: string | undefined;
  to: string | undefined;
}

export type User = {
  fullName: string;
  email: string;
}

export type City = {
  id: number;
  name: string;
  latitude: number;
  longitude: number;
}

export type Cinema = {
  id: number;
  name: string;
  address: string;
  latitude: number;
  longitude: number;
}

export type Showtime = {
  id: number;
  startTime: string;
  endTime: string;
  movieId: number;
  movieTitle: string;
  posterPath: string;
  hallId: number;
  hallName: string;
  cinemaId: number;
  cinemaName: string;
}

export type ShowtimeDay = {
  date: string;
  showtimes: Showtime[];
}
