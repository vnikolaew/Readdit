import {
   AuthenticationResultErrorModel,
   Community,
   PostApiCommunitiesBody,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import communityClient from "../client";

const createCommunity = async (model: PostApiCommunitiesBody) => {
   await sleep(500);

   const { data, headers, status } = await communityClient.postForm<Community>(
      "/",
      model
   );

   if (status === HttpStatusCode.BadRequest) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useCreateCommunityMutation = () => {
   return useMutation(createCommunity, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
