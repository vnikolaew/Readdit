import { AuthenticationResultErrorModel, FeedCommunityPostModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import postsClient from "../client";

const getAllByUser = async (userId: string) => {
   await sleep(2000);

   const { data, status } = await postsClient.get<
      FeedCommunityPostModel[]
   >(`/user/${userId}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetAllPostsByUserQuery = (userId?: string) => {
   return useQuery(
      ["user", userId, "posts"],
      () => getAllByUser(userId!),
      {
         onError: console.error,
         onSuccess: (data) => {
         },
         onSettled: (res) => console.log(res),
         enabled: !!userId,
      },
   );
};
