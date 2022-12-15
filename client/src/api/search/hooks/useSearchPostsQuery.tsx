import {
   AuthenticationResultErrorModel,
   PostSearchModel,
   TimeRange,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import searchClient from "../client";

const searchPosts = async (query: string, range: TimeRange) => {
   await sleep(500);

   const { data, headers, status } = await searchClient.get<PostSearchModel[]>(
      `/posts?query=${query}&range=${range}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useSearchPostsQuery = (query: string, range: TimeRange) => {
   return useQuery(
      ["search", "posts", query, range] as const,
      ({ queryKey: [_, __, query, range] }) => searchPosts(query, range),
      {
         onError: console.error,
         onSuccess: ({ data }) => {},
         onSettled: (res) => console.log(res),
         cacheTime: 60 * 1000,
         staleTime: 30 * 1000,
         refetchOnWindowFocus: false,
         enabled: false,
      }
   );
};
