import {
   AuthenticationResultErrorModel,
   FeedCommunityPostModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import postFeedClient from "../client";

const getMostRecentPosts = async () => {
   await sleep(500);

   const { data, status } = await postFeedClient.get<FeedCommunityPostModel[]>(
      `/new`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetMostRecentPostsQuery = (
   options?: Omit<
      UseQueryOptions<
         FeedCommunityPostModel[],
         ApiError,
         FeedCommunityPostModel[],
         string[]
      >,
      "queryKey" | "queryFn" | "initialData"
   > & { initialData?: () => undefined }
) => {
   return useQuery(["feed", "new"], getMostRecentPosts, {
      onError: console.error,
      onSuccess: (data) => {},
      onSettled: (res) => console.log(res),
      cacheTime: 60 * 1000,
      staleTime: 30 * 1000,
      refetchOnWindowFocus: false,
      ...options,
   });
};
