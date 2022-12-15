import {
   AuthenticationResultErrorModel,
   CommunitySearchModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import searchClient from "../client";

const searchCommunities = async (query: string) => {
   await sleep(500);

   const { data, headers, status } = await searchClient.get<
      CommunitySearchModel[]
   >(`/communities?query=${query}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useSearchCommuntiesQuery = (query: string) => {
   return useQuery(
      ["search", "communities", query] as const,
      ({ queryKey: [_, __, query] }) => searchCommunities(query),
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
