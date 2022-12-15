import {
   AuthenticationResultErrorModel,
   PutApiUsersBody,
   UserDetailsModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import usersClient from "../client";

const updateUserProfile = async (payload: PutApiUsersBody) => {
   await sleep(500);

   const { data, headers, status } = await usersClient.put<UserDetailsModel>(
      `/`
   );

   if (status !== HttpStatusCode.Accepted) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useUpdateUserProfileMutation = () => {
   return useMutation(updateUserProfile, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
