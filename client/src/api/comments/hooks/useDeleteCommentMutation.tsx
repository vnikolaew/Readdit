import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentsClient from "../client";

const deleteComment = async (commentId: string) => {
   await sleep(500);
   const { data, headers, status } = await commentsClient.delete<boolean>(
      `/${commentId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError(["Comment could not deleted!"]);
   }

   return { data, headers, status };
};

export const useDeleteCommentMutation = (commentId: string) => {
   return useMutation(() => deleteComment(commentId), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
