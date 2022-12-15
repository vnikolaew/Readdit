/**
 * Generated by orval v6.10.3 🍺
 * Do not edit manually.
 * Readdit
 * OpenAPI spec version: v1
 */
import type { ApplicationUser } from "./applicationUser";
import type { Community } from "./community";
import type { UserCommunityStatus } from "./userCommunityStatus";

export interface UserCommunity {
   userId: string;
   user?: ApplicationUser;
   communityId: string;
   community?: Community;
   status: UserCommunityStatus;
   createdOn?: string;
   modifiedOn?: string | null;
   isDeleted?: boolean;
   deletedOn?: string | null;
}
