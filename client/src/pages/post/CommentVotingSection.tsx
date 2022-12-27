import React, { FC } from "react";
import { ChevronDownIcon, ChevronUpIcon } from "@chakra-ui/icons";
import { log } from "../../utils/logger";
import { UserVoteModel } from "../../api/models";
import { useUpvoteCommentMutation } from "../../api/commentVotes/hooks/useUpvoteCommentMutation";
import { useDownvoteCommentMutation } from "../../api/commentVotes/hooks/useDownvoteCommentMutation";
import { useRemoveUpvoteCommentMutation } from "../../api/commentVotes/hooks/useRemoveCommentUpvoteMutation";
import { useRemoveDownvoteCommentMutation } from "../../api/commentVotes/hooks/useRemoveCommentDownvoteMutation";
import { Flex, FlexProps, Text, useMantineTheme } from "@mantine/core";
import { showNotification } from "@mantine/notifications";

interface IProps extends FlexProps {
   userVote: UserVoteModel;
   postId: string;
   commentId: string;
   voteScore: number;
   orientation: "horizontal" | "vertical";
}

const CommentVotingSection: FC<IProps> = ({
   userVote,
   postId,
   orientation,
   commentId,
   voteScore,
   ...rest
}) => {
   const theme = useMantineTheme();
   const { mutateAsync: upVoteAsync } = useUpvoteCommentMutation(postId);
   const { mutateAsync: downVoteAsync } = useDownvoteCommentMutation(postId);
   const { mutateAsync: removeUpVoteAsync } = useRemoveUpvoteCommentMutation(postId);
   const { mutateAsync: removeDownVoteAsync } = useRemoveDownvoteCommentMutation(postId);

   const hasUserUpvoted = userVote?.type === "Up";
   const hasUserDownvoted = userVote?.type === "Down";

   return (
      <Flex
         px={1}
         bg={"transparent"}
         justify={"flex-start"}
         align={"center"}
         gap={4}
         direction={orientation === "horizontal" ? "row" : "column"}
         {...rest}
      >
         <ChevronUpIcon
            // mt={6}
            borderRadius={"full"}
            transition={"200ms"}
            onClick={async (e) => {
               e.preventDefault();

               let response;
               if (hasUserUpvoted) {
                  response = await removeUpVoteAsync(commentId);
               } else {
                  response = await upVoteAsync(commentId);
                  showNotification({
                     title: "UpVote successful.",
                     message: "You've upvoted this post.",
                     color: theme.colors.green[3],
                     autoClose: 3000,
                     disallowClose: false,
                  });
               }
               log(response);
            }}
            color={hasUserUpvoted ? theme.colors.red[9] : theme.colors.dark[0]}
            bgColor={hasUserUpvoted ? theme.colors.dark[6] : "transparent"}
            _hover={{ bgColor: theme.colors.dark[6], color: theme.colors.red[9] }}
            cursor={"pointer"}
            boxSize={8}
         />
         <Text pt={0.5} fz={16} color={theme.colors.gray[0]}>
            {voteScore}
         </Text>
         <ChevronDownIcon
            borderRadius={"full"}
            transition={"200ms"}
            onClick={async (e) => {
               e.preventDefault();

               let response;
               if (hasUserDownvoted) {
                  response = await removeDownVoteAsync(commentId);
               } else {
                  response = await downVoteAsync(commentId);
                  showNotification({
                     title: "DownVote successful.",
                     message: "You've downvoted this post.",
                     color: theme.colors.green[3],
                     autoClose: 3000,
                     disallowClose: false,
                  });
               }
               log(response);
            }}
            color={hasUserDownvoted ? "facebook.700" : theme.colors.dark[0]}
            bgColor={hasUserDownvoted ? theme.colors.dark[6] : "transparent"}
            _hover={{ bgColor: theme.colors.dark[6], color: "facebook.700" }}
            cursor={"pointer"}
            boxSize={8}
         />
      </Flex>
   );
};

export default CommentVotingSection;