import {
   CommunityDetailsModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import communityClient from "../client";

const getCommunityDetails = async (communityIdOrName: string) => {
   await sleep(500);

   const { data, status } =
      await communityClient.get<CommunityDetailsModel>(`/${communityIdOrName}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as ApiError).errors!);
   }

   return data;
};

export const useGetCommunityDetailsQuery = (communityIdOrName?: string | null) => {
   return useQuery(
      ["community", communityIdOrName],
      ({ queryKey: [_, communityIdOrName] }) => getCommunityDetails(communityIdOrName!),
      {
         onError: console.error,
         onSuccess: (data) => {},
         onSettled: (res) => console.log(res),
         cacheTime: 10 * 60 * 1000,
         staleTime: 10 * 60 * 1000,
         enabled: !!communityIdOrName
      }
   );
};
