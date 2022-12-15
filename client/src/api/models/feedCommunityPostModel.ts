/**
 * Generated by orval v6.10.3 🍺
 * Do not edit manually.
 * Readdit
 * OpenAPI spec version: v1
 */
import type { FeedPostAuthorModel } from "./feedPostAuthorModel";
import type { FeedCommunityModel } from "./feedCommunityModel";
import type { UserVoteModel } from "./userVoteModel";

export interface FeedCommunityPostModel {
   author?: FeedPostAuthorModel;
   community?: FeedCommunityModel;
   id?: string | null;
   mediaUrl?: string | null;
   mediaPublicId?: string | null;
   title?: string | null;
   content?: string | null;
   createdOn?: string;
   modifiedOn?: string | null;
   voteScore?: number;
   commentsCount?: number;
   userVote?: UserVoteModel;
}