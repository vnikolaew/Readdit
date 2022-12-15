import { UserCommunityModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import userCommunityClient from "../client";

const getUserCommunities = async (userId: string) => {
   await sleep(500);

   const { data, status } =
      await userCommunityClient.get<UserCommunityModel[] | ApiError>(`/${userId}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as ApiError).errors!);
   }

   return data as UserCommunityModel[];
};

export const useGetMyCommunitiesQuery = (userId?: string | null) => {
   return useQuery(
      ["communities", userId] as const,
      () => getUserCommunities(userId!),
      {
         onError: console.error,
         onSuccess: (data) => {},
         onSettled: (res) => console.log(res),
         cacheTime: 10 * 60 * 1000,
         staleTime: 5 * 60 * 1000,
         enabled: !!userId,
         refetchOnWindowFocus: false,
      }
   );
};
