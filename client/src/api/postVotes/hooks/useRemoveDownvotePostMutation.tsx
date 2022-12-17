import { AuthenticationResultErrorModel, FeedCommunityPostModel, PostVote } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import postVotesClient from "../client";

const removeDownvote = async (postId: string) => {
   await sleep(500);

   const { data, status } = await postVotesClient.delete<PostVote>(
      `/down/${postId}`,
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useRemoveDownvotePostMutation = () => {
   const queryClient = useQueryClient();
   const recentPostsFeedKey = ["feed", "new"] as const;

   return useMutation(removeDownvote, {
      onMutate: async (postId) => {
         await queryClient.cancelQueries(recentPostsFeedKey);

         const posts = queryClient.getQueryData<FeedCommunityPostModel[]>(recentPostsFeedKey);
         queryClient.setQueryData(recentPostsFeedKey, (prevPosts: FeedCommunityPostModel[] | undefined) => {
               return (prevPosts || []).map(p => {
                  return p.id === postId ? {
                     ...p, userVote: null,
                     voteScore: p.voteScore! + 1,
                  } : p;
               }) as FeedCommunityPostModel[];
            },
         );
         return { posts };
      },
      onError: (err, vars, context) => {
         queryClient.setQueryData(recentPostsFeedKey, context!.posts!);
      },
      onSuccess: (data) => {
      },
      onSettled: (res) => console.log(res),
   });
};
