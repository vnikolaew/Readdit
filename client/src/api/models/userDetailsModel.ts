/**
 * Generated by orval v6.10.3 🍺
 * Do not edit manually.
 * Readdit
 * OpenAPI spec version: v1
 */
import type { UserProfileModel } from "./userProfileModel";

export interface UserDetailsModel {
   id?: string | null;
   userName?: string | null;
   firstName?: string | null;
   lastName?: string | null;
   email?: string | null;
   createdOn?: string;
   profile?: UserProfileModel;
   postsScore?: number;
   commentsScore?: number;
}
