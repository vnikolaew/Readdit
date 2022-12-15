import { AuthenticationResultErrorModel, PostVote } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import postVotesClient from "../client";

const removeDownvote = async (postId: string) => {
   await sleep(500);

   const { data, headers, status } = await postVotesClient.delete<PostVote>(
      `/down/${postId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useRemoveDownvotePostMutation = () => {
   return useMutation(removeDownvote, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
