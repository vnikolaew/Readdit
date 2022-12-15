import {
   AuthenticationResultErrorModel,
   AuthenticationResultSuccessModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import identityClient from "../client";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const registerUser = async (model: FormData) => {
   await sleep(500);

   const { data, status } = await identityClient.postForm<
      AuthenticationResultSuccessModel | AuthenticationResultErrorModel
   >("register", model);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useRegisterMutation = () => {
   const queryClient = useQueryClient();
   const userKey = "user";

   return useMutation(registerUser, {
      onError: console.error,
      onSuccess: (data) => {
         queryClient.setQueryData([userKey], data);
      },
      onSettled: (res) => console.log(res),
      cacheTime: 30 * 60 * 1000,
   });
};
