import React, { FC } from "react";
import { Flex, FlexProps, Text, useToast } from "@chakra-ui/react";
import { ChevronDownIcon, ChevronUpIcon } from "@chakra-ui/icons";
import { log } from "../../utils/logger";
import { UserVoteModel } from "../../api/models";
import { useUpvoteCommentMutation } from "../../api/commentVotes/hooks/useUpvoteCommentMutation";
import { useDownvoteCommentMutation } from "../../api/commentVotes/hooks/useDownvoteCommentMutation";
import { useRemoveUpvoteCommentMutation } from "../../api/commentVotes/hooks/useRemoveCommentUpvoteMutation";
import { useRemoveDownvoteCommentMutation } from "../../api/commentVotes/hooks/useRemoveCommentDownvoteMutation";

interface IProps extends FlexProps {
   userVote: UserVoteModel;
   postId: string;
   commentId: string;
   voteScore: number;
   orientation: "horizontal" | "vertical";
}

const CommentVotingSection: FC<IProps> = ({ userVote, postId, orientation, commentId, voteScore, ...rest }) => {
   const toast = useToast();
   const { mutateAsync: upVoteAsync } = useUpvoteCommentMutation(postId);
   const { mutateAsync: downVoteAsync } = useDownvoteCommentMutation(postId);
   const { mutateAsync: removeUpVoteAsync } = useRemoveUpvoteCommentMutation(postId);
   const { mutateAsync: removeDownVoteAsync } = useRemoveDownvoteCommentMutation(postId);

   const hasUserUpvoted = userVote?.type === "Up";
   const hasUserDownvoted = userVote?.type === "Down";

   return (
      <Flex
         px={1}
         borderLeftRadius={6}
         bgColor={"gray.900"}
         justifyContent={"flex-start"}
         alignItems={"center"}
         gap={2}
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
                  toast({
                     title: "UpVote successful.",
                     description: "You've upvoted this post.",
                     status: "success",
                     variant: "subtle",
                     duration: 3000,
                     isClosable: true,
                  });
               }
               log(response);
            }}
            color={hasUserUpvoted ? "red" : "inherit"}
            bgColor={hasUserUpvoted ? "whiteAlpha.100" : "transparent"}
            _hover={{ bgColor: "whiteAlpha.100", color: "red" }}
            cursor={"pointer"}
            boxSize={8} />
         <Text pt={.5} fontSize={16} color={"white"}>{voteScore}</Text>
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
                  toast({
                     title: "DownVote successful.",
                     description: "You've downvoted this post.",
                     variant: "subtle",
                     status: "success",
                     duration: 3000,
                     isClosable: true,
                  });
               }
               log(response);
            }}
            color={hasUserDownvoted ? "facebook.700" : "inherit"}
            bgColor={hasUserDownvoted ? "whiteAlpha.100" : "transparent"}
            _hover={{ bgColor: "whiteAlpha.100", color: "facebook.700" }}
            cursor={"pointer"}
            boxSize={8} />
      </Flex>
   );
};

export default CommentVotingSection;