import {
   AuthenticationResultErrorModel,
   AuthenticationResultSuccessModel,
   PostApiIdentityConfirmEmailParams,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import identityClient from "../client";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";

const confirmEmail = async ({
   userId,
   token,
}: PostApiIdentityConfirmEmailParams) => {
   await sleep(500);

   const { data, headers, status } = await identityClient.post<
      AuthenticationResultSuccessModel | AuthenticationResultErrorModel
   >(`confirm?userId=${userId}&token=${token}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useConfirmEmailMutation = () => {
   return useMutation(confirmEmail, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
