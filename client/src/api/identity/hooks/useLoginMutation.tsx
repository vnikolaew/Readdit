import {
   AuthenticationResultErrorModel,
   AuthenticationResultSuccessModel,
   PostApiIdentityLoginBody,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import identityClient from "../client";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const loginUser = async (model: PostApiIdentityLoginBody) => {
   await sleep(500);

   const { data, status } = await identityClient.post<
      AuthenticationResultSuccessModel | AuthenticationResultErrorModel
   >("login", model);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useLoginMutation = () => {
   const queryClient = useQueryClient();
   const userKey = "user";

   return useMutation(loginUser, {
      onError: console.error,
      onSuccess: (data) => {
         queryClient.setQueryData([userKey], data);
      },
      onSettled: (res) => console.log(res),
      cacheTime: 60 * 60 * 1000,
   });
};
