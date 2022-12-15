import { useQueryClient } from "@tanstack/react-query";
import { useCurrentUser } from "../../common/hooks/useCurrentUser";

export const useSignOutMutation = () => {
   const queryClient = useQueryClient();
   const user = useCurrentUser();

   return async () => {
      queryClient.removeQueries(["user", "details", user?.userId]);
      queryClient.setQueryData(["user"], null);
      queryClient.setQueryData([["user", "details", user?.userId]], null);

      await queryClient.removeQueries({ predicate: (_) => true });
   };
};
