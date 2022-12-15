import { AuthenticationResultErrorModel, UserDetailsModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import usersClient from "../client";

const getUserDetails = async (userId: string) => {
   await sleep(500);

   const { data, status } = await usersClient.get<UserDetailsModel>(
      `/${userId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetUserDetailsQuery = (userId?: string | null) => {
   return useQuery(
      ["user", "details", userId] as const,
      ({ queryKey: [_, __, userId] }) => {
         return !!userId ? getUserDetails(userId!) : null;
      },
      {
         onError: console.error,
         onSuccess: (data) => {},
         onSettled: (res) => console.log(res),
         cacheTime: 10 * 60 * 1000,
         staleTime: 5 * 60 * 1000,
         refetchOnWindowFocus: false,
      }
   );
};
