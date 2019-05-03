import React, { Component, ReactNode } from "react";
import InfiniteScroll from "react-infinite-scroller";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import FlatService from "../../services/FlatService";
import { IApiResponse } from "../../services/interfaces/Common";
import { IShortFlatDetails } from "../../services/interfaces/FlatServiceInterfaces";
import FlatBox, { FlatBoxLoader } from "./FlatBox";
import Styles from "./FlatList.module.css";

class FlatList extends Component<
  RouteComponentProps,
  { pageSize: number; page: number; hasMore: boolean; flats: IShortFlatDetails[] }
> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = { pageSize: 20, page: 0, hasMore: true, flats: [] };
    this.loadFlats(this.state.page);
  }

  public render() {
    return (
      <FlexRow className={Styles.flatBoxes}>
        {/* <FlatFilters onPageCountChange={console.log} onShowRentedChange={this.handleShowRentedChange} /> */}
        <InfiniteScroll
          className={Styles.scroller}
          pageStart={0}
          initialLoad={false}
          loadMore={this.loadFlats}
          hasMore={this.state.hasMore}
          loader={this.getLoaderItems(4)}
          useWindow={true}
        >
          {this.getFlatItems()}
        </InfiniteScroll>
      </FlexRow>
    );
  }

  private getFlatItems(): ReactNode[] {
    const flats = this.state.flats.map((flat) => <FlatBox key={flat.id} flat={flat} />);
    if (flats.length > 0) {
      return flats;
    } else if (this.state.hasMore) {
      return Array(12)
        .fill(0)
        .map((_, idx) => <FlatBoxLoader key={idx} />);
    } else {
      return [<></>];
    }
  }

  private getLoaderItems(count: number) {
    const els = Array(count)
      .fill(0)
      .map((_, idx) => <FlatBoxLoader key={idx} />);
    return <>{els}</>;
  }

  private loadFlats = (pageNumber: number) => {
    FlatService.getFlats(this.state.pageSize, this.state.pageSize * pageNumber)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  };

  private handleFlatResult = (result: IApiResponse<IShortFlatDetails[]>) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.data !== undefined) {
      this.setState((state) => ({ flats: [...state.flats, ...result.data!], hasMore: result.data!.length !== 0 }));
    }
  };

  private handleFail() {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default FlatList;
