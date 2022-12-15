import { AuthenticationResultErrorModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import postsClient from "../client";

const deletePost = async (postId: string) => {
   await sleep(500);

   const { data, headers, status } = await postsClient.delete(`/${postId}`);

   if (status !== HttpStatusCode.NoContent) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useDeletePostMutation = (postId: string) => {
   return useMutation(() => deletePost(postId), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
