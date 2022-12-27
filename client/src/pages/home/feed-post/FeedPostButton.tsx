import { Box, BoxProps, Button, useMantineTheme } from "@mantine/core";
import React, { FC, PropsWithChildren } from "react";
import { IconType } from "react-icons";

interface IProps extends PropsWithChildren, BoxProps {
   Icon: IconType;
   path: string;
}

const FeedPostButton: FC<IProps> = ({ path, Icon, children, ...rest }) => {
   const theme = useMantineTheme();
   return (
      <Box {...rest}>
         <Button
            radius={"md"}
            styles={theme => ({
               root: {
                  "&:hover": {
                     backgroundColor: theme.colors.dark[6],
                  },
               },
            })}
            bg={"transparent"}
            color={theme.colors.gray[9]}
            leftIcon={<Icon size={16} color={theme.colors.gray[0]} />}>
            {children}
         </Button>
      </Box>
   );
};

export default FeedPostButton;