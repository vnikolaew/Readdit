import { AuthenticationResultErrorModel, UserCommunity } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import userCommunityClient from "../client";

const approveUserCommunity = async (communityId: string, userId: string) => {
   await sleep(500);

   const { data, headers, status } =
      await userCommunityClient.post<UserCommunity>(
         `/approve/${communityId}/${userId}`
      );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useApproveUserCommunityMutation = (
   communityId: string,
   userId: string
) => {
   return useMutation(() => approveUserCommunity(communityId, userId), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
