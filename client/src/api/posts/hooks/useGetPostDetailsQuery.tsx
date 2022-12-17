import { AuthenticationResultErrorModel, PostDetailsModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import postsClient from "../client";

const getPostDetails = async (postId: string) => {
   await sleep(500);

   const { data, status } = await postsClient.get<PostDetailsModel>(
      `/${postId}`,
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetPostDetailsQuery = (postId: string) => {
   return useQuery(
      ["post", postId],
      ({ queryKey: [_, postId] }) => getPostDetails(postId),
      {
         onError: console.error,
         onSuccess: (data) => {
         },
         onSettled: (res) => console.log(res),
         cacheTime: 10 * 60 * 1000,
      },
   );
};
