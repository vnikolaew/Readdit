import { AuthenticationResultErrorModel, UserCommunity } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import userCommunityClient from "../client";

const getUserCommunityRelationship = async (communityId: string) => {
   await sleep(500);

   const { data, headers, status } =
      await userCommunityClient.get<UserCommunity>(`/relationship/${communityId}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useGetUserCommunityRelationship = (communityId: string) => {
   return useQuery(
      ["community", "relationship", communityId] as const,
      ({ queryKey: [_, __, communityId] }) =>
         getUserCommunityRelationship(communityId),
      {
         onError: console.error,
         onSuccess: ({ data }) => {},
         onSettled: (res) => console.log(res),
         cacheTime: 10 * 60 * 1000,
         staleTime: 5 * 60 * 1000,
         refetchOnWindowFocus: false,
      }
   );
};
