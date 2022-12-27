import React, { FC, forwardRef, useRef, useState } from "react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { PostApiPostsBody, UserCommunityModel } from "../../api/models";
import { useGetMyCommunitiesQuery } from "../../api/userCommunities/hooks/useGetMyCommunitiesQuery";
import { FastField, FieldProps } from "formik";
import {
   Avatar,
   Box,
   Group,
   Select,
   Stack,
   Text,
   useMantineTheme,
} from "@mantine/core";
import { ChevronDownIcon } from "@chakra-ui/icons";
import { TbCircleDotted } from "react-icons/tb";

const CommunitySelectDropdown: FC = () => {
   const user = useCurrentUser();
   const selectRef = useRef<HTMLInputElement | null>(null);

   const theme = useMantineTheme();
   const [selectedCommunityId, setSelectedCommunityId] = useState<string>();
   const { data: communities } = useGetMyCommunitiesQuery(user?.userId);

   return (
      <FastField name={"CommunityId"}>
         {({
            field: { name },
            form: { setFieldValue },
         }: FieldProps<PostApiPostsBody>) => (
            <Select
               w={"40%"}
               ref={selectRef!}
               size={"md"}
               styles={(theme) => ({
                  input: {
                     backgroundColor: theme.colors.dark[5],
                     borderColor: theme.colors.gray[2],
                     borderWidth: 1,
                     borderRadius: 6,
                     color: theme.colors.gray[0],
                  },
                  itemsWrapper: {
                     marginBlock: 0,
                     backgroundColor: theme.colors.dark[5],
                     color: theme.colors.gray[0],
                     borderRadius: 6,
                  },
                  label: {
                     textAlign: "left",
                     fontSize: 16,
                     marginBottom: 6,
                  },
                  item: {
                     backgroundColor: theme.colors.dark[5],
                     color: theme.colors.gray[0],
                     "&:hover": {
                        backgroundColor: theme.colors.dark[4],
                        color: theme.colors.gray[0],
                     },
                     "&[data-selected]": {
                        "&, &:hover": {
                           backgroundColor: theme.colors.dark[5],
                           color: theme.colors.gray[0],
                        },
                     },
                  },
               })}
               placeholder={"Choose a community"}
               label={"Choose a community"}
               variant={"filled"}
               rightSection={
                  <ChevronDownIcon
                     color={theme.colors.gray[0]}
                     onClick={(_) => selectRef.current?.select()}
                     cursor={"pointer"}
                     fontSize={14}
                  />
               }
               icon={
                  !!selectedCommunityId?.length ? (
                     <Avatar
                        radius={"lg"}
                        size={28}
                        mx={8}
                        src={
                           communities?.find(
                              (c) => c.id === selectedCommunityId
                           )!.pictureUrl
                        }
                     />
                  ) : (
                     <TbCircleDotted size={28} />
                  )
               }
               iconWidth={50}
               value={selectedCommunityId}
               onChange={(id) => {
                  setFieldValue(name, id!);
                  setSelectedCommunityId(id!);
               }}
               clearable
               searchable
               maxDropdownHeight={200}
               data={
                  communities?.map((c) => ({
                     ...c,
                     group: "YOUR COMMUNITIES",
                     value: c.id!,
                     label: `r/${c.name}`,
                  })) || []
               }
            />
         )}
      </FastField>
   );
};

const SelectItem = forwardRef<HTMLDivElement, UserCommunityModel>((c, ref) => {
   const theme = useMantineTheme();
   return (
      <Box
         ref={ref}
         component={"div"}
         sx={(_) => ({
            cursor: "pointer",
            "&:hover": {
               backgroundColor: theme.colors.dark[6],
            },
         })}
         key={c.id}
         px={12}
         my={4}
         py={8}
         bg={"transparent"}
      >
         <Group spacing={"sm"}>
            <Avatar radius={"xl"} size={"sm"} src={c!.pictureUrl!} />
            <Stack spacing={0}>
               <Text size={"sm"} color={theme.colors.dark[0]}>
                  c/{c.name}
               </Text>
               <Text pl={4} size={"xs"} color={theme.colors.dark[0]}>
                  · {c.membersCount} members
               </Text>
            </Stack>
         </Group>
      </Box>
   );
});

export default CommunitySelectDropdown;
