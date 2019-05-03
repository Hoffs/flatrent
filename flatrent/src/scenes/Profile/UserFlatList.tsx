import React, { Component, ReactNode } from "react";
import InfiniteScroll from "react-infinite-scroller";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import { IApiResponse } from "../../services/interfaces/Common";
import { IShortFlatDetails } from "../../services/interfaces/FlatServiceInterfaces";
import UserService from "../../services/UserService";
import FlatBox, { FlatBoxLoader } from "../Flats/FlatBox";
import Styles from "./UserFlatList.module.css";

interface IUserFlatListProps {
  userId: string;
}

interface IUserFlatlistState {
  pageSize: number;
  page: number;
  hasMore: boolean;
  flats?: IShortFlatDetails[];
}

class UserFlatList extends Component<IUserFlatListProps, IUserFlatlistState> {
  private loadFlatsFunc: (page: number) => void;

  constructor(props: IUserFlatListProps) {
    super(props);
    this.state = { pageSize: 16, page: 0, hasMore: true, flats: undefined };
    this.loadFlatsFunc = this.getLoadFlats(this.props.userId);
    this.loadFlats(this.state.page);
  }

  public componentWillReceiveProps(newProps: IUserFlatListProps) {
    this.setState({ flats: undefined });
    this.loadFlatsFunc = this.getLoadFlats(newProps.userId);
    this.loadFlatsFunc(0);
  }

  public render() {
    return (
      <FlexRow className={Styles.flatBoxes}>
        {/* <FlatFilters onPageCountChange={console.log} onShowRentedChange={this.handleShowRentedChange} /> */}
        <InfiniteScroll
          className={Styles.scroller}
          pageStart={0}
          initialLoad={false}
          loadMore={this.loadFlatsFunc}
          hasMore={this.state.hasMore}
          loader={this.getLoaderItems()}
          useWindow={true}
        >
          {this.getFlatItems()}
        </InfiniteScroll>
      </FlexRow>
    );
  }

  private getFlatItems(): ReactNode[] {
    if (this.state.flats !== undefined) {
      const flats = this.state.flats.map((flat) => <FlatBox key={flat.id} flat={flat} />);
      return flats;
    } else {
      return Array(12)
        .fill(0)
        .map((_, idx) => <FlatBoxLoader key={idx} />);
    }
  }

  private getLoaderItems() {
    // return Array(count).fill(0).map((_, idx) => <FlatBoxLoader key={idx} />);
    return <FlatBoxLoader key={0} />;
  }

  private loadFlats = (pageNumber: number) => {
    UserService.getUserFlats(this.props.userId, this.state.pageSize * pageNumber)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  };

  private getLoadFlats = (userId: string) => (pageNumber: number) => {
    UserService.getUserFlats(userId, this.state.pageSize * pageNumber)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  };

  private handleFlatResult = (result: IApiResponse<IShortFlatDetails[]>) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.data !== undefined) {
      this.setState((state) => {
        // { flats: [...state.flats, ...result.data!], hasMore: result.data!.length !== 0 }
        const newState: { hasMore: boolean; flats: IShortFlatDetails[] } = {
          hasMore: result.data!.length === state.pageSize,
          flats: [],
        };
        if (state.flats !== undefined) {
          newState.flats.push(...state.flats);
        }
        newState.flats.push(...result.data!);
        return newState;
      });
    }
  };

  private handleFail() {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default UserFlatList;
