import React, { FC } from "react";
import { Box, Flex } from "@chakra-ui/react";
import FeedType from "./FeedType";
import { AiOutlineWifi } from "react-icons/ai";
import { IoMdRocket } from "react-icons/io";
import { FeedRankBy, PostFeedType } from "./post-feed/PostFeed";

interface IProps {
   feedType: PostFeedType;
   setFeedType: React.Dispatch<React.SetStateAction<PostFeedType>>;
}

const FeedTypeSelector: FC<IProps> = ({ setFeedType, feedType }) => {

   return (
      <Box
         borderWidth={1}
         borderRadius={6}
         borderColor="gray"
         width={"full"}
         _active={{ bgColor: "blackAlpha.900" }}
         color={"white"}
         bgColor={"blackAlpha.900"}
      >
         <Flex gap={2} px={4} py={3} justifyContent={"flex-start"} alignItems={"center"}>
            <FeedType
               onClick={_ => {
                  setFeedType(ft => ({ ...ft, type: FeedRankBy.Recent }));
               }}
               isActive={feedType.type === FeedRankBy.Recent}
               Icon={AiOutlineWifi}>
               Recent
            </FeedType>
            <FeedType
               onClick={_ => {
                  setFeedType(ft => ({ ...ft, type: FeedRankBy.Best }));
               }}
               isActive={feedType.type === FeedRankBy.Best}
               Icon={IoMdRocket}>
               Best
            </FeedType>
         </Flex>
      </Box>
   )
      ;
};

export default FeedTypeSelector;