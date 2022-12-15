import {
   AuthenticationResultErrorModel,
   PostComment,
   UpdateCommentInputModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentsClient from "../client";

const updateComment = async (
   commentId: string,
   payload: UpdateCommentInputModel
) => {
   await sleep(500);
   const { data, headers, status } = await commentsClient.put<PostComment>(
      `/${commentId}`,
      payload
   );

   if (status !== HttpStatusCode.Accepted) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useUpdateCommentMutation = (commentId: string) => {
   return useMutation((payload) => updateComment(commentId, payload), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
