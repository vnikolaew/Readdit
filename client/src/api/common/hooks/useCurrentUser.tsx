import { useQuery, useQueryClient } from "@tanstack/react-query";
import { AuthenticationResultSuccessModel } from "../../models";

export const useCurrentUser = (): AuthenticationResultSuccessModel | null => {
   const userKey = "user";
   const queryClient = useQueryClient();
   const { data: authResult } = useQuery<AuthenticationResultSuccessModel>([
      userKey,
   ]);

   if (authResult === undefined) return null;

   const expirationTime = authResult.token!.expiresAt!;
   if (Date.parse(expirationTime) < Date.now()) {
      queryClient.removeQueries([userKey], { exact: true });
      return null;
   }

   return authResult;
};
