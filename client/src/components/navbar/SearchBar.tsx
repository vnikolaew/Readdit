import React, { FC, useEffect, useState } from "react";
import { FiSearch } from "react-icons/fi";
import { useDebounce } from "../../utils/useDebounce";
import { useSearchCommuntiesQuery } from "../../api/search/hooks/useSearchCommunitiesQuery";
import { log } from "../../utils/logger";
import { AiOutlineCloseCircle } from "react-icons/ai";
import {
   Autocomplete,
   Avatar,
   Box,
   Container,
   Group,
   Stack,
   Text,
   useMantineTheme,
} from "@mantine/core";
import { CommunitySearchModel } from "../../api/models";
import Link from "../Link";

const SearchBar: FC = () => {
   const [searchTerm, setSearchTerm] = useState<string>("");
   const debouncedSearch = useDebounce(searchTerm, 2000, (text) => text.length >= 2);
   const { refetch, data: communities } = useSearchCommuntiesQuery(debouncedSearch!);
   const theme = useMantineTheme();

   useEffect(() => {
      if (!debouncedSearch.length || !searchTerm.length) return;
      refetch({ queryKey: ["search", "communities", debouncedSearch] }).then(({ data }) => {
         log(data);
      });
   }, [debouncedSearch, refetch]);

   return (
      <Container bg={"transparent"}>
         <Autocomplete
            w={400}
            value={searchTerm}
            onChange={setSearchTerm}
            // variant={"default"}
            styles={(theme) => ({
               input: {
                  backgroundColor: theme.colors.dark[5],
                  borderColor: "transparent",
                  borderWidth: 0,
                  color: theme.colors.gray[0],
                  "&:hover": {
                     backgroundColor: theme.colors.gray[9],
                     borderColor: theme.colors.gray[0],
                     borderWidth: 1,
                  },
                  "&:active": {
                     backgroundColor: theme.colors.gray[9],
                     borderColor: theme.colors.gray[0],
                     borderWidth: 1,
                  },
                  "&:focus": {
                     backgroundColor: theme.colors.gray[9],
                     borderColor: theme.colors.gray[0],
                     borderWidth: 1,
                  },
               },
               dropdown: {
                  backgroundColor: theme.colors.gray[9],
                  border: "none",
               },
            })}
            px={2}
            placeholder={"Search Readdit"}
            rightSection={
               searchTerm.length > 0 && (
                  <AiOutlineCloseCircle
                     color={theme.colors.gray[0]}
                     cursor={"pointer"}
                     onClick={() => setSearchTerm("")}
                     size={20}
                  />
               )
            }
            itemComponent={(c: CommunitySearchModel) => (
               <Box
                  sx={(_) => ({
                     // borderRadius: "10px",
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
                  <Link to={`/c/${c.name}`}>
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
                  </Link>
               </Box>
            )}
            size={"md"}
            radius={"xl"}
            bg={theme.colors.dark[6]}
            icon={<FiSearch style={{ paddingInline: 2 }} size={22} />}
            data={
               communities?.data.map((c) => ({
                  ...c,
                  group: "Communities",
                  value: c.name!,
               })) ?? ([] as (CommunitySearchModel & { value: string })[])
            }
         />
      </Container>
   );
};

export default SearchBar;