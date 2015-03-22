template <typename Char, typename Traits, typename Tuple, std::size_t I>
void printTuple(basic_ostream<Char, Traits>& out, Tuple const& t, int_<I>);

template <typename Char, typename Traits, typename Tuple>
void printTuple(basic_ostream<Char, Traits>& out, Tuple const& t, int_<0>);

template <typename Tuple>
void printTupleVector(const vector<Tuple>& v);

template <typename Char, typename Traits, typename... Args>
ostream& operator<<(basic_ostream<Char, Traits>& out, tuple<Args...> const& t);

template <typename T>
ostream& operator<<(ostream& out, const unique_ptr<T>& ptr);



template<typename Char, typename Traits, typename Tuple, size_t I>
void printTuple(basic_ostream<Char, Traits>& out, Tuple const& t, int_<I>)
{
    printTuple(out, t, int_<I - 1>());
    out << ", " << get<I>(t);
}

template<typename Char, typename Traits, typename Tuple>
void printTuple(basic_ostream<Char, Traits>& out, Tuple const& t, int_<0>)
{
    out << get<0>(t);
}

template <typename Tuple>
void printTupleVector(const vector<Tuple>& v)
{
#if __GNUC__ >= 4 || (__GNUC__ == 4 && __GNUC_MINOR__ >= 6)
    for (const auto& item : v)
    {
        cout << item << endl;
    }
#elif __GNUC__ >= 4 || (__GNUC__ == 4 && __GNUC_MINOR__ >= 4)
    auto end = v.cend();
    for (auto item(v.cbegin()); item != end; ++item)
    {
        cout << *item<< endl;
    }
#else
    vector<Tuple>::const_iterator end(users.end());
    for (
        vector<Tuple>::const_iterator item(v.begin());
        item != end;
        ++item
    )
    {
        cout << *item << endl;
    }
#endif
}

template<typename Char, typename Traits, typename... Args>
ostream& operator<<(
    basic_ostream<Char, Traits>& out,
    tuple<Args...> const& t)
{
    out << "(";
    printTuple(out, t, int_<sizeof...(Args) - 1>());
    out << ")";
    return out;
}

template <typename T>
ostream& operator<<(ostream& out, const unique_ptr<T>& ptr)
{
    if (nullptr != ptr.get())
    {
        out << *ptr;
    }
    else
    {
        out << "NULL";
    }
    return out;
}
