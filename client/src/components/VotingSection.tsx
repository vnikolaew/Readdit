import React, { FC } from "react";
import { Flex, FlexProps, Text, useToast } from "@chakra-ui/react";
import { ChevronDownIcon, ChevronUpIcon } from "@chakra-ui/icons";
import { log } from "../utils/logger";
import { useUpvotePostMutation } from "../api/postVotes/hooks/useUpvotePostMutation";
import { useDownvotePostMutation } from "../api/postVotes/hooks/useDownvotePostMutation";
import { useRemoveUpvotePostMutation } from "../api/postVotes/hooks/useRemoveUpvotePostMutation";
import { useRemoveDownvotePostMutation } from "../api/postVotes/hooks/useRemoveDownvotePostMutation";
import { UserVoteModel } from "../api/models";

interface IProps extends FlexProps {
   userVote: UserVoteModel;
   postId: string;
   voteScore: number;
   orientation: "horizontal" | "vertical";
}

const PostVotingSection: FC<IProps> = ({ userVote, orientation, postId, voteScore, ...rest }) => {
   const toast = useToast();
   const { mutateAsync: upVoteAsync } = useUpvotePostMutation();
   const { mutateAsync: downVoteAsync } = useDownvotePostMutation();
   const { mutateAsync: removeUpVoteAsync } = useRemoveUpvotePostMutation();
   const { mutateAsync: removeDownVoteAsync } = useRemoveDownvotePostMutation();

   const hasUserUpvoted = userVote?.type === "Up";
   const hasUserDownvoted = userVote?.type === "Down";

   return (
      <Flex
         px={1}
         borderLeftRadius={6}
         bgColor={"gray.900"}
         justifyContent={"flex-start"}
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
                  response = await removeUpVoteAsync(postId);
               } else {
                  response = await upVoteAsync(postId);
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
         <Text fontSize={16} color={"white"}>{voteScore}</Text>
         <ChevronDownIcon
            borderRadius={"full"}
            transition={"200ms"}
            onClick={async (e) => {
               e.preventDefault();

               let response;
               if (hasUserDownvoted) {
                  response = await removeDownVoteAsync(postId);
               } else {
                  response = await downVoteAsync(postId);
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

export default PostVotingSection;