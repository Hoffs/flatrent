import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader"
import FlexColumn from "../../components/FlexColumn";
import { avatarUrl } from "../../services/ApiUtilities";
import { IUserDetails } from "../../services/Settings";
import Styles from "./UserInfo.module.css";
import { Link } from "react-router-dom";
import { userProfileUrl, conversationWithUserUrl } from "../../utilities/Utilities";

const UserDisplay = ({user}: {user?: IUserDetails}) => {
  if (user === undefined) {
    return <UserDisplayLoader />;
  }

  return (
    <FlexColumn className={Styles.userInfo}>
      <Link className={Styles.contactLink} to={userProfileUrl(user.id)}>
      <span className={Styles.avatarName}>
        {user.firstName}
      </span>
      <div className={Styles.avatarWrapper}><img className={Styles.avatar} src={avatarUrl(user.id)} /></div>
      </Link>
      <span className={Styles.contactOwner}>
        <Link className={Styles.contactLink} to={conversationWithUserUrl(user.id)}>Siųsti žinutę</Link>
      </span>
    </FlexColumn>
  );
};

const UserDisplayLoader = () => (
  <ContentLoader
    style={{ width: "80px", height: "150px" }}
    height={150}
    width={80}
    speed={2}
    primaryColor="#f3f3f3"
    secondaryColor="#ecebeb"
  >
    <rect x="0" y="15" rx="3" ry="3" width="80" height="6" />
    <rect x="0" y="130" rx="3" ry="3" width="80" height="6" />
    <circle cx="40" cy="75" r="40" />
  </ContentLoader>
);

export default UserDisplay;