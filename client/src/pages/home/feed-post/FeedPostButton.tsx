import React, { FC, PropsWithChildren } from "react";
import { Box, BoxProps, Button } from "@chakra-ui/react";
import { IconType } from "react-icons";

interface IProps extends PropsWithChildren, BoxProps {
   Icon: IconType;
   path: string;
}

const FeedPostButton: FC<IProps> = ({ path, Icon, children, ...rest }) => {
   return (
      <Box {...rest}>
         <Button borderRadius={6} _hover={{ bgColor: "whiteAlpha.100" }}
                 bgColor={"transparent"}
                 color={"gray"}
                 leftIcon={<Icon size={16} color={"gray"} />}>
            {children}
         </Button>
      </Box>
   );
};

export default FeedPostButton;