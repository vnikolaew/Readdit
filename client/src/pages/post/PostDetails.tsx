import React, { FC } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useGetPostDetailsQuery } from "../../api/posts/hooks/useGetPostDetailsQuery";
import { log } from "../../utils/logger";
import VotingSection from "../../components/VotingSection";
import { ImImage } from "react-icons/im";
import { shorten } from "../../utils/string";
import FeedPost from "../home/feed-post/FeedPost";
import { CloseIcon } from "@chakra-ui/icons";
import CommentsSection from "./CommentsSection";
import { Box, Button, Center, Divider, Flex, Loader, Text, useMantineTheme } from "@mantine/core";

const PostDetails: FC = () => {
   const { postId } = useParams();
   const navigate = useNavigate();
   const theme = useMantineTheme();
   const { data: post, isLoading } = useGetPostDetailsQuery(postId!);

   if (isLoading || !post) {
      return (
         <Center w={"100%"}>
            <Loader size={"md"} color={theme.colors.blue[4]} />
         </Center>
      );
   }

   log(post);
   return (
      <Box mb={24} w={"70%"} mx={"auto"}>
         <Flex w={"100%"} mx={"auto"} align={"center"} direction={"column"} gap={12}>
            <Flex
               bg={theme.colors.dark[9]}
               style={{
                  position: "sticky",
                  zIndex: 100,
                  borderBottomLeftRadius: 6,
                  borderBottomRightRadius: 6,
               }}
               top={-1}
               justify={"center"}
               py={8}
               px={22}
               w={"100%"}
               gap={6}
               align={"center"}
            >
               <Divider
                  my={"auto"}
                  size={"sm"}
                  h={16}
                  color={theme.colors.dark[6]}
                  orientation={"vertical"}
               />
               <VotingSection
                  userVote={post.userVote!}
                  postId={post.id!}
                  bg={"transparent"}
                  gap={4}
                  color={"white"}
                  voteScore={post.voteScore!}
                  orientation={"horizontal"}
               />
               <Divider
                  my={"auto"}
                  size={"sm"}
                  h={16}
                  color={theme.colors.dark[6]}
                  orientation={"vertical"}
               />
               {post.mediaUrl && (
                  <Box mx={8}>
                     <ImImage size={16} color={"white"} />
                  </Box>
               )}
               <Text color={theme.colors.gray[0]}>{shorten(post.title!, 50)}</Text>
               <Button
                  ml={"auto"}
                  radius={"lg"}
                  styles={(theme) => ({
                     root: {
                        borderColor: "transparent",
                        "&:hover": {
                           backgroundColor: theme.colors.dark[6],
                        },
                        transitionDuration: "200ms",
                     },
                  })}
                  bg={"transparent"}
                  onClick={() => navigate(-1)}
                  fz={12}
                  color={theme.colors.gray[0]}
                  leftIcon={<CloseIcon boxSize={10} color={theme.colors.gray[0]} />}
               >
                  Close
               </Button>
            </Flex>
            <FeedPost w={"600px"} post={{ ...post, commentsCount: post.comments!.length }} />
            <CommentsSection postId={post.id!} comments={post.comments!} />
         </Flex>
      </Box>
   );
};

export default PostDetails;
