import React, { FC } from "react";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { ArrowRightIcon } from "@chakra-ui/icons";
import { useNavigate } from "react-router-dom";
import { sleep } from "../../utils/sleep";
import { Avatar, Button, Flex, Indicator, Input, useMantineTheme } from "@mantine/core";

const CreatePostSection: FC = () => {
   const user = useCurrentUser();
   const navigate = useNavigate();
   const theme = useMantineTheme();
   const { data: userInfo, isLoading } = useGetUserDetailsQuery(user?.userId);

   return (
      <Flex
         style={{
            borderWidth: 1,
            borderRadius: 6,
            cursor: "pointer",
            borderColor: theme.colors.gray[9],
         }}
         styles={theme => ({
            "&:hover": {
               bgColor: theme.colors.dark[7],
            },
         })}
         align={"center"}
         direction={"row"}
         onClick={async () => {
            await sleep(1000);
            navigate("/create/post");
         }}
         w={"550px"}
         gap={8}
         py={8}
         px={6}
         color={theme.colors.dark[0]}
         bg={theme.colors.dark[9]}
      >
         <Indicator
            dot
            inline
            color="teal"
            position="bottom-end"
            offset={4}
            size={10}
         >
            <Avatar
               radius={"xl"}
               size={"md"}
               src={userInfo!.profile!.pictureUrl!}
            />
         </Indicator>
         <Input
            styles={theme => ({
               input: {
                  borderWidth: 1,
                  borderColor: theme.colors.gray[6],
                  backgroundColor: theme.colors.dark[5],
                  "&:hover": {
                     backgroundColor: theme.colors.dark[8],
                  },
               },
            })}
            w={"70%"}
            size={"md"}
            variant={"filled"}
            placeholder={"Create Post"}
         />
         <Button
            onClick={async () => {
               await sleep(200);
               navigate("/create/post");
            }}
            ml={20}
            styles={theme => ({
               root: {
                  borderColor: "transparent",
                  borderWidth: 1,
                  "&:hover": {
                     backgroundColor: theme.colors.dark[8],
                     borderWidth: 1,
                     borderColor: theme.colors.gray[6],
                  },
               },
            })}
            bg={"transparent"}
         >
            <ArrowRightIcon />
         </Button>
      </Flex>
   );
};

export default CreatePostSection;
