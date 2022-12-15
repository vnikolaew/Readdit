import {
   AuthenticationResultErrorModel,
   Community,
   PutApiCommunitiesCommunityIdBody,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import communityClient from "../client";

const updateCommunity = async (
   communityId: string,
   payload: PutApiCommunitiesCommunityIdBody
) => {
   await sleep(500);

   const { data, headers, status } = await communityClient.put<Community>(
      `/${communityId}`,
      payload
   );

   if (status === HttpStatusCode.Accepted) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useUpdateCommunityMutation = (communityId: string) => {
   return useMutation((payload) => updateCommunity(communityId, payload), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
