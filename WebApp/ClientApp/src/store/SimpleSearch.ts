import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface SearchState {
    isLoading: boolean;
    searchTerm: string;    
    searchResults: SearchResult[];
}

export interface SearchResult {
    url: string;
    label: number;
    searchEngine: SearchProvider[];
}

export enum SearchProvider {
    Google,
    Bing,
    DuckDuckGo
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface IncrementCountAction { type: 'INCREMENT_COUNT_SS' }
export interface DecrementCountAction { type: 'DECREMENT_COUNT_SS' }

interface RequestSearchResultsAction {
    type: 'REQUEST_SEARCH_RESULTS';
    searchTerm: string;
}

interface ReceiveSearchResultsAction {
    type: 'RECEIVE_SEARCH_RESULTS';
    searchResults: SearchResult[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = IncrementCountAction | DecrementCountAction | RequestSearchResultsAction | ReceiveSearchResultsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestSearchResults: (searchTerm: string): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({ type: 'REQUEST_SEARCH_RESULTS', searchTerm: searchTerm });

        fetch(`api/search?searchTerm=` + searchTerm)
           .then(response => response.json() as Promise<SearchResult[]>)
           .then(data => {
               dispatch({ type: 'RECEIVE_SEARCH_RESULTS', searchResults: data });
               });
    }    
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<SearchState> = (state: SearchState | undefined, incomingAction: Action): SearchState => {
    if (state === undefined) {
        return { searchResults: [], isLoading: false, searchTerm: ""};
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_SEARCH_RESULTS':
            return Object.assign({}, state, {
                searchTerm: action.searchTerm,
                searchResults: [],
                isLoading: true
            });
        case 'RECEIVE_SEARCH_RESULTS':
            return Object.assign({}, state, {
                searchResults: action.searchResults,
                isLoading: false
            });
        default:
            return state;
    }
};
