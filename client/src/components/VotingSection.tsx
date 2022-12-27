import React, { FC } from "react";
import { ChevronDownIcon, ChevronUpIcon } from "@chakra-ui/icons";
import { log } from "../utils/logger";
import { useUpvotePostMutation } from "../api/postVotes/hooks/useUpvotePostMutation";
import { useDownvotePostMutation } from "../api/postVotes/hooks/useDownvotePostMutation";
import { useRemoveUpvotePostMutation } from "../api/postVotes/hooks/useRemoveUpvotePostMutation";
import { useRemoveDownvotePostMutation } from "../api/postVotes/hooks/useRemoveDownvotePostMutation";
import { UserVoteModel } from "../api/models";
import { ActionIcon, Flex, FlexProps, Text, useMantineTheme } from "@mantine/core";
import { showNotification } from "@mantine/notifications";

interface IProps extends FlexProps {
   userVote: UserVoteModel;
   postId: string;
   voteScore: number;
   orientation: "horizontal" | "vertical";
}

const PostVotingSection: FC<IProps> = ({ userVote, orientation, postId, voteScore, ...rest }) => {
   const theme = useMantineTheme();
   const { mutateAsync: upVoteAsync } = useUpvotePostMutation();
   const { mutateAsync: downVoteAsync } = useDownvotePostMutation();
   const { mutateAsync: removeUpVoteAsync } = useRemoveUpvotePostMutation();
   const { mutateAsync: removeDownVoteAsync } = useRemoveDownvotePostMutation();

   const hasUserUpvoted = userVote?.type === "Up";
   const hasUserDownvoted = userVote?.type === "Down";

   return (
      <Flex
         py={8}
         my={0}
         bg={theme.colors.dark[9]}
         justify={"flex-start"}
         align={"center"}
         gap={2}
         direction={orientation === "horizontal" ? "row" : "column"}
         {...rest}
      >
         <ActionIcon
            color={hasUserUpvoted ? theme.colors.red[6] : theme.colors.dark[0]}
            onClick={async (e) => {
               e.preventDefault();

               let response;
               if (hasUserUpvoted) {
                  response = await removeUpVoteAsync(postId);
               } else {
                  response = await upVoteAsync(postId);
                  showNotification({
                     title: "UpVote successful.",
                     message: "You've upvoted this post.",
                     autoClose: 3000,
                     color: theme.colors.green[3],
                  });
               }
               log(response);
            }}
            bg={hasUserUpvoted ? theme.colors.dark[6] : "transparent"}
            sx={(theme) => ({
               transitionDuration: "100ms",
               "&:hover": {
                  bg: theme.colors.dark[0],
                  color: theme.colors.red[6],
               },
               cursor: "pointer",
            })}
            radius={"lg"}
         >
            <ChevronUpIcon
               color={hasUserUpvoted ? theme.colors.red[6] : theme.colors.dark[0]}
               _hover={{ color: theme.colors.red[6] }}
               boxSize={24}
            />
         </ActionIcon>
         <Text fz={16} color={theme.colors.gray[0]}>
            {voteScore}
         </Text>
         <ActionIcon
            color={hasUserDownvoted ? theme.colors.blue[6] : theme.colors.dark[0]}
            onClick={async (e) => {
               e.preventDefault();

               let response;
               if (hasUserDownvoted) {
                  response = await removeDownVoteAsync(postId);
               } else {
                  response = await downVoteAsync(postId);
                  showNotification({
                     title: "DownVote successful.",
                     message: "You've downvoted this post.",
                     autoClose: 3000,
                     color: theme.colors.green[3],
                  });
               }
               log(response);
            }}
            bg={hasUserDownvoted ? theme.colors.dark[6] : "transparent"}
            sx={(theme) => ({
               transitionDuration: "100ms",
               "&:hover": {
                  bg: theme.colors.dark[0],
                  color: theme.colors.blue[6],
               },
               cursor: "pointer",
            })}
            radius={"lg"}
         >
            <ChevronDownIcon
               color={hasUserDownvoted ? theme.colors.blue[6] : theme.colors.dark[0]}
               _hover={{ color: theme.colors.blue[6] }}
               boxSize={24}
            />
         </ActionIcon>
      </Flex>
   );
};

export default PostVotingSection;
