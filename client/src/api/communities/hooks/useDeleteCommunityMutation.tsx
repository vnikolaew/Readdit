import { AuthenticationResultErrorModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import communityClient from "../client";

const deleteCommunity = async (communityId: string) => {
   await sleep(500);

   const { data, headers, status } = await communityClient.delete(
      `/${communityId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useDeleteCommunityMutation = (communityId: string) => {
   return useMutation(() => deleteCommunity(communityId), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
