#ifndef SINGLETON_H
#define SINGLETON_H

template <class T>
class Singleton
{
public:
    static T& GetInstance();
    bool Initialsize();

protected:
    Singleton() {}

private:
    Singleton( const Singleton& );
};

template <class T>
T& Singleton<T>::GetInstance()
{
    static T instanz;
    return instanz;
}
#endif // SINGLETON_H
