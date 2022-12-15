import {
   AuthenticationResultErrorModel,
   FeedCommunityPostModel,
   TimeRange,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import postFeedClient from "../client";

const getBestVotedPosts = async (range: TimeRange) => {
   await sleep(500);

   const { data, status } = await postFeedClient.get<
      FeedCommunityPostModel[]
   >(`/top?range=${range}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetBestVotedPostsQuery = (range: TimeRange) => {
   return useQuery(["feed", "top", range], () => getBestVotedPosts(range), {
      onError: console.error,
      onSuccess: (data) => {},
      onSettled: (res) => console.log(res),
      cacheTime: 60 * 1000,
      staleTime: 30 * 1000,
      refetchOnWindowFocus: false,
   });
};
