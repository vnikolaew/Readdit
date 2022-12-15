import { AuthenticationResultErrorModel, UserCommunity } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import userCommunityClient from "../client";

const leaveCommunity = async (communityId: string) => {
   await sleep(500);

   const { data, headers, status } =
      await userCommunityClient.post<UserCommunity>(`/leave/${communityId}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useLeaveCommunityMutation = (communityId: string) => {
   return useMutation(() => leaveCommunity(communityId), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
