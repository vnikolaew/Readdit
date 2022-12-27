import React, { FC } from "react";
import FeedType from "./FeedType";
import { AiOutlineWifi } from "react-icons/ai";
import { IoMdRocket } from "react-icons/io";
import { FeedRankBy, PostFeedType } from "./post-feed/PostFeed";
import { Box, Flex, useMantineTheme } from "@mantine/core";

interface IProps {
   feedType: PostFeedType;
   setFeedType: React.Dispatch<React.SetStateAction<PostFeedType>>;
}

const FeedTypeSelector: FC<IProps> = ({ setFeedType, feedType }) => {
   const theme = useMantineTheme();

   return (
      <Box
         sx={{
            borderWidth: 1,
            borderRadius: 6,
            borderColor: "gray",
            "&:hover": {
               bgColor: theme.colors.dark[9],
            },
         }}
         w={"100%"}
         color={theme.colors.gray[0]}
         bg={theme.colors.dark[9]}
      >
         <Flex gap={12} p={12} justify={"flex-start"} align={"center"}>
            <FeedType
               onClick={(_) => {
                  setFeedType((ft) => ({ ...ft, type: FeedRankBy.Recent }));
               }}
               isActive={feedType.type === FeedRankBy.Recent}
               Icon={AiOutlineWifi}
            >
               Recent
            </FeedType>
            <FeedType
               onClick={(_) => {
                  setFeedType((ft) => ({ ...ft, type: FeedRankBy.Best }));
               }}
               isActive={feedType.type === FeedRankBy.Best}
               Icon={IoMdRocket}
            >
               Best
            </FeedType>
         </Flex>
      </Box>
   );
};

export default FeedTypeSelector;
