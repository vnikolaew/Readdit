import React, { FC } from "react";
import { FeedCommunityPostModel } from "../../../api/models";
import Link from "../../../components/Link";
import moment from "moment";
import FeedPostButton from "./FeedPostButton";
import { FaRegComment } from "react-icons/fa";
import VotingSection from "../../../components/VotingSection";
import {
   Avatar,
   Box,
   Container,
   Flex,
   FlexProps,
   Image,
   Text,
   useMantineTheme,
} from "@mantine/core";

interface IProps extends FlexProps {
   post: FeedCommunityPostModel;
}

const FeedPost: FC<IProps> = ({ post, ...rest }) => {
   const theme = useMantineTheme();

   return (
      <Flex
         sx={(theme) => ({
            borderWidth: 1,
            borderColor: "transparent",
            borderRadius: 8,
            "&:hover": {
               borderColor: theme.colors.gray[6],
               borderWidth: 1,
            },
         })}
         bg={theme.colors.dark[9]}
         w={"550px"}
         align={"stretch"}
         {...rest}
      >
         <Container
            bg={theme.colors.dark[9]}
            sx={{
               borderWidth: 1,
               borderColor: "transparent",
               borderRadius: 8,
            }}
            h={"100%"}
         >
            <VotingSection
               mt={3}
               userVote={post.userVote!}
               postId={post.id! || ""}
               voteScore={post.voteScore!}
               orientation={"vertical"}
            />
         </Container>
         <Flex
            style={{ flexGrow: 1 }}
            p={12}
            sx={{
               borderWidth: 1,
               borderColor: "transparent",
               borderRadius: 8,
            }}
            color={theme.colors.dark[0]}
            bg={theme.colors.dark[8]}
            direction={"column"}
         >
            <Flex color={theme.colors.gray[6]} align={"center"} gap={8}>
               <Avatar
                  style={{ zIndex: 1 }}
                  size={"sm"}
                  radius={"lg"}
                  src={post.community?.pictureUrl!}
               />
               <Link to={`/c/${post.community?.id || ""}`}>
                  <Text
                     span
                     color={theme.colors.gray[0]}
                     sx={{
                        "&:hover": {
                           textDecorationLine: "underline",
                        },
                     }}
                     fz={13}
                  >
                     c/{post.community?.name!}
                  </Text>
               </Link>
               <Text span color={theme.colors.gray[6]} fz={12}>
                  ·
               </Text>
               <Text span color={theme.colors.gray[6]} fz={12}>
                  Posted by{" "}
                  <Link to={`/user/${post.author!.id || ""}`}>
                     <Text
                        pr={4}
                        sx={{
                           "&:hover": {
                              textDecorationLine: "underline",
                           },
                        }}
                        display={"inline"}
                     >
                        u/{post.author?.userName!}
                     </Text>
                  </Link>
                  {moment(Date.parse(post!.createdOn!)).fromNow()}
               </Text>
            </Flex>
            <Text color={theme.colors.gray[0]} my={1} ta={"left"} fz={20}>
               {post.title!}
            </Text>
            <Text color={theme.colors.gray[0]} mb={12} ta={"left"} fz={16}>
               {post.content!}
            </Text>
            {post.mediaUrl && (
               <Flex mb={8} justify={"center"} align={"center"} w={"full"} mx={-14}>
                  <Image w={200} fit={"cover"} radius={"sm"} src={post.mediaUrl} />
               </Flex>
            )}
            <Box style={{ alignSelf: "flex-start" }}>
               <FeedPostButton
                  p={1}
                  my={1}
                  ml={-4}
                  Icon={FaRegComment}
                  path={`/post/${post.id || ""}`}
               >
                  {post.commentsCount} comment{post.commentsCount !== 1 && "s"}
               </FeedPostButton>
            </Box>
         </Flex>
      </Flex>
   );
};

export default FeedPost;
