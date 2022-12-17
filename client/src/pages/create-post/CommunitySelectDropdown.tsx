import { ChevronDownIcon } from "@chakra-ui/icons";
import {
   Avatar,
   Button,
   Flex,
   HStack,
   Menu,
   MenuButton,
   MenuGroup,
   MenuItem,
   MenuList,
   Text,
   VStack,
} from "@chakra-ui/react";
import React, { FC, useState } from "react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { PostApiPostsBody, UserCommunityModel } from "../../api/models";
import { useGetMyCommunitiesQuery } from "../../api/userCommunities/hooks/useGetMyCommunitiesQuery";
import { FastField, FieldProps } from "formik";

const CommunitySelectDropdown: FC = () => {
   const user = useCurrentUser();
   const [selectedCommunity, setSelectedCommunity] =
      useState<UserCommunityModel>();
   const { data: communities } = useGetMyCommunitiesQuery(user?.userId);

   return (
      <FastField name={"CommunityId"}>
         {({ field: { name }, form: { setFieldValue } }: FieldProps<PostApiPostsBody>) => (
            <Menu>
               <MenuButton
                  borderWidth={2}
                  borderColor={"gray"}
                  width={"30%"}
                  _active={{ bgColor: "blackAlpha.800" }}
                  py={6}
                  px={4}
                  bgColor={"blackAlpha.800"}
                  color={"white"}
                  fontSize={12}
                  _hover={{
                     borderColor: "gray",
                     borderWidth: 2,
                  }}
                  as={Button}
                  textAlign={"start"}
                  rightIcon={<ChevronDownIcon ml={4} boxSize={6} />}
               >
                  {selectedCommunity ? (
                     <HStack spacing={2}>
                        <Avatar
                           borderRadius={"50%"}
                           width={8}
                           height={8}
                           objectFit={"cover"}
                           src={selectedCommunity.pictureUrl!}
                        />
                        <Text fontWeight={"medium"} color={"white"} fontSize={14}>
                           c/{selectedCommunity.name}
                        </Text>
                     </HStack>
                  ) : (
                     "Choose a community"
                  )}
               </MenuButton>
               <MenuList py={0} border={"none"}>
                  <MenuGroup
                     textAlign={"start"}
                     fontSize={10}
                     title={"YOUR COMMUNITIES"}
                  >
                     {communities?.map((c) => (
                        <MenuItem
                           borderRadius={10}
                           onClick={() => {
                              setSelectedCommunity(c);
                              setFieldValue(name, c.id);
                           }}
                           px={3}
                           py={3}
                           gap={6}
                           _hover={{ bgColor: "transparent" }}
                        >
                           <Flex
                              justifyContent={"flex-start"}
                              alignItems={"center"}
                              gap={3}
                           >
                              <Avatar
                                 borderRadius={"50%"}
                                 width={8}
                                 height={8}
                                 objectFit={"cover"}
                                 src={c!.pictureUrl!}
                              />
                              <VStack spacing={0}>
                                 <Text color={"black"} fontSize={14}>
                                    c/{c.name}
                                 </Text>
                                 <Text color={"gray"} fontSize={10}>
                                    {c.membersCount} members
                                 </Text>
                              </VStack>
                           </Flex>
                        </MenuItem>
                     ))}
                  </MenuGroup>
               </MenuList>
            </Menu>
         )}
      </FastField>
   );
};

export default CommunitySelectDropdown;