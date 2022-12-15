import { AuthenticationResultErrorModel, CommentVote } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentVotesClient from "../../comments/client";

const downvoteComment = async (commentId: string) => {
   await sleep(500);

   const { data, headers, status } = await commentVotesClient.post<CommentVote>(
      `/down/${commentId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useDownvoteCommentMutation = () => {
   return useMutation(downvoteComment, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
